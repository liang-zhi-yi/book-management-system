import tkinter as tk
from tkinter import ttk, messagebox
from bll.user_manager import UserManager


class FrmLogin:
    def __init__(self):
        self.root = tk.Tk()
        self.root.title("图书管理系统 - 登录")
        self.root.geometry("400x300")
        self.root.resizable(False, False)
        self.current_user = None
        self.user_manager = UserManager()
        self.setup_ui()

    def setup_ui(self):
        tk.Label(self.root, text="图书管理系统", font=("Arial", 20)).place(x=100, y=30)
        tk.Label(self.root, text="用户名:", font=("Arial", 12)).place(x=60, y=90)
        tk.Label(self.root, text="密码:", font=("Arial", 12)).place(x=60, y=130)
        tk.Label(self.root, text="用户类型:", font=("Arial", 12)).place(x=60, y=170)

        self.txt_user_name = tk.Entry(self.root, font=("Arial", 12), width=20)
        self.txt_user_name.place(x=130, y=90)
        self.txt_password = tk.Entry(self.root, font=("Arial", 12), width=20, show="*")
        self.txt_password.place(x=130, y=130)

        self.cmb_user_type = ttk.Combobox(self.root, font=("Arial", 12), width=18, state="readonly")
        self.cmb_user_type['values'] = ("系统管理员", "读者")
        self.cmb_user_type.current(0)
        self.cmb_user_type.place(x=130, y=170)

        self.btn_login = tk.Button(self.root, text="登录", font=("Arial", 12), width=10, command=self.btn_login_click)
        self.btn_login.place(x=130, y=220)

        self.txt_user_name.insert(0, "admin")
        self.txt_password.insert(0, "123456")

    def btn_login_click(self):
        user_name = self.txt_user_name.get().strip()
        password = self.txt_password.get()
        user_type = self.cmb_user_type.current()

        if not user_name:
            messagebox.showwarning("提示", "请输入用户名！")
            self.txt_user_name.focus()
            return
        if not password:
            messagebox.showwarning("提示", "请输入密码！")
            self.txt_password.focus()
            return

        try:
            user = self.user_manager.login(user_name, password)

            if user.user_type != user_type:
                expected_type = "系统管理员" if user_type == 0 else "读者"
                actual_type = "系统管理员" if user.user_type == 0 else "读者"
                messagebox.showwarning("提示", f"用户类型不匹配！\n你选择的是【{expected_type}】，但该账户是【{actual_type}】！")
                return

            self.current_user = user
            messagebox.showinfo("系统提示", f"登录成功！欢迎 {user.real_name}！")
            self.root.destroy()

        except Exception as ex:
            messagebox.showerror("登录失败", str(ex))

    def show(self):
        self.root.mainloop()
        return self.current_user