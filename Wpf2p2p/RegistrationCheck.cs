namespace Wpf2p2p
{
	class RegistrationCheck
    {
		public string LadderValidation(string login, string password, string confirm, int region_id)
		{
			if (!IsFieldsCompleted(login, password, confirm, region_id))
				return "Поля не заполнены";
			if (!IsDesiredLength(login, password))
				return "Недопустимая длина данных";
			if (!IsPasswordEqual(password, confirm))
				return "Пароли не совпадают";
			if (!IsFreeLogin(login))
				return "Логин уже занят";

			return "Вы зарегистрировались";
		}

		public bool IsFieldsCompleted(string login, string password, string confirm, int region_id)
		{
			if (login.Equals("") || password.Equals("") || confirm.Equals("") || region_id == -1)
				return false;
			else
				return true;
		}

		public bool IsDesiredLength(string login, string password)
		{
			if (login.Length < 3 || password.Length < 3)
				return false;
			else
				return true;
		}

		public bool IsPasswordEqual(string password, string confirm)
		{
			if (!password.Equals(confirm))
				return false;
			else
				return true;
		}

		public bool IsFreeLogin(string login, int id = 0)
		{
			return DBConnection.FreeLogin(login, id);
		}
	}
}