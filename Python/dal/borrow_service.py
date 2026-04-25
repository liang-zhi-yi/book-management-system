from dal.db_helper import DBHelper
from datetime import datetime


class BorrowService:
    def __init__(self):
        self.db = DBHelper()

    def borrow_book(self, reader_id: int, book_id: int, borrow_date: str, due_date: str) -> bool:
        conn = self.db.get_connection()
        cursor = conn.cursor()
        try:
            cursor.execute("SELECT Status FROM Reader WHERE ReaderId=?", (reader_id,))
            reader_row = cursor.fetchone()
            if not reader_row or reader_row['Status'] != 0:
                return False

            cursor.execute("SELECT AvailableCount FROM Book WHERE BookId=?", (book_id,))
            book_row = cursor.fetchone()
            if not book_row or book_row['AvailableCount'] <= 0:
                return False

            cursor.execute("""
                INSERT INTO BorrowRecord (ReaderId, BookId, BorrowDate, DueDate, Status)
                VALUES (?, ?, ?, ?, 0)
            """, (reader_id, book_id, borrow_date, due_date))

            cursor.execute("UPDATE Book SET AvailableCount = AvailableCount - 1 WHERE BookId=?", (book_id,))

            conn.commit()
            return True
        except Exception as e:
            conn.rollback()
            print(f"借书失败：{e}")
            return False
        finally:
            conn.close()

    def get_reader_borrow_info(self, reader_id: int) -> list:
        sql = """
            SELECT br.BorrowId, br.BorrowDate, br.DueDate, b.BookName, b.ISBN, r.ReaderName, r.ReaderCategoryId
            FROM BorrowRecord br
            INNER JOIN Book b ON br.BookId = b.BookId
            INNER JOIN Reader r ON br.ReaderId = r.ReaderId
            WHERE br.ReaderId=? AND br.Status IN (0, 2)
            ORDER BY br.DueDate
        """
        rows, _ = self.db.execute_data_set(sql, (reader_id,))
        return rows

    def search_borrow_records(self, reader_id: int = None, isbn: str = None, borrow_id: int = None, status: int = None) -> list:
        sql = """
            SELECT br.BorrowId, br.BorrowDate, br.DueDate, br.ReturnDate, br.Status,
                   b.BookName, b.ISBN, r.ReaderName, r.ReaderId
            FROM BorrowRecord br
            INNER JOIN Book b ON br.BookId = b.BookId
            INNER JOIN Reader r ON br.ReaderId = r.ReaderId
            WHERE 1=1
        """
        params = []
        if status is not None:
            sql += " AND br.Status=?"
            params.append(status)
        else:
            sql += " AND br.Status IN (0, 2)"
        if reader_id:
            sql += " AND r.ReaderId=?"
            params.append(reader_id)
        if isbn:
            sql += " AND b.ISBN LIKE ?"
            params.append(f"%{isbn}%")
        if borrow_id:
            sql += " AND br.BorrowId=?"
            params.append(borrow_id)
        sql += " ORDER BY br.DueDate"
        rows, _ = self.db.execute_data_set(sql, tuple(params) if params else ())
        return rows

    def return_book(self, borrow_id: int, return_date: str, late_fee: float, remark: str) -> bool:
        conn = self.db.get_connection()
        cursor = conn.cursor()
        try:
            cursor.execute("SELECT BookId FROM BorrowRecord WHERE BorrowId=?", (borrow_id,))
            row = cursor.fetchone()
            if not row:
                return False
            book_id = row['BookId']

            cursor.execute("""
                UPDATE BorrowRecord
                SET ReturnDate=?, LateFee=?, Status=1, Remark=?
                WHERE BorrowId=?
            """, (return_date, late_fee, remark, borrow_id))

            cursor.execute("UPDATE Book SET AvailableCount = AvailableCount + 1 WHERE BookId=?", (book_id,))

            conn.commit()
            return True
        except Exception as e:
            conn.rollback()
            print(f"还书失败：{e}")
            return False
        finally:
            conn.close()

    def get_borrow_record_by_id(self, borrow_id: int) -> list:
        sql = """
            SELECT br.*, b.BookName, b.ISBN, r.ReaderName, r.ReaderCategoryId
            FROM BorrowRecord br
            INNER JOIN Book b ON br.BookId = b.BookId
            INNER JOIN Reader r ON br.ReaderId = r.ReaderId
            WHERE br.BorrowId=?
        """
        rows, _ = self.db.execute_data_set(sql, (borrow_id,))
        return rows

    def search_all_borrow_records(self, reader_id: int = None, reader_name: str = None,
                                   book_name: str = None, isbn: str = None,
                                   status: int = None, borrow_from: str = None,
                                   borrow_to: str = None, return_from: str = None,
                                   return_to: str = None, page_index: int = 1,
                                   page_size: int = 10) -> tuple:
        sql = """
            SELECT br.BorrowId, br.BorrowDate, br.DueDate, br.ReturnDate, br.Status, br.LateFee, br.Remark,
                   CASE
                       WHEN br.Status = 0 THEN '借出中'
                       WHEN br.Status = 1 THEN '已归还'
                       WHEN br.Status = 2 THEN '逾期'
                       ELSE '未知'
                   END as StatusText,
                   b.BookName, b.ISBN, b.BookId, r.ReaderName, r.ReaderId
            FROM BorrowRecord br
            INNER JOIN Book b ON br.BookId = b.BookId
            INNER JOIN Reader r ON br.ReaderId = r.ReaderId
            WHERE 1=1
        """
        params = []

        if reader_id is not None and reader_id != 0:
            sql += " AND r.ReaderId=?"
            params.append(reader_id)
        if reader_name:
            sql += " AND r.ReaderName LIKE ?"
            params.append(f"%{reader_name}%")
        if book_name:
            sql += " AND b.BookName LIKE ?"
            params.append(f"%{book_name}%")
        if isbn:
            sql += " AND b.ISBN LIKE ?"
            params.append(f"%{isbn}%")
        if status is not None:
            sql += " AND br.Status=?"
            params.append(status)
        if borrow_from:
            sql += " AND br.BorrowDate>=?"
            params.append(borrow_from)
        if borrow_to:
            sql += " AND br.BorrowDate<=?"
            params.append(borrow_to)
        if return_from:
            sql += " AND br.ReturnDate>=?"
            params.append(return_from)
        if return_to:
            sql += " AND br.ReturnDate<=?"
            params.append(return_to)

        count_sql = f"SELECT COUNT(*) FROM ({sql})"
        total = self.db.execute_scalar(count_sql, tuple(params))

        sql += " ORDER BY br.BorrowDate DESC"
        if page_size > 0:
            offset = (page_index - 1) * page_size
            sql += f" LIMIT {page_size} OFFSET {offset}"

        rows, columns = self.db.execute_data_set(sql, tuple(params))
        return rows, total