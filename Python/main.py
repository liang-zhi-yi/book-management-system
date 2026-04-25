import sys
import os

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))

from dal import db
from ui.login import FrmLogin
from ui.main import FrmMain


def main():
    login = FrmLogin()
    user = login.show()
    if user:
        main_form = FrmMain(user)
        main_form.show()


if __name__ == "__main__":
    main()