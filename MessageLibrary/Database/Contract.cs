namespace MessageLibrary.Database
{
    public class Contract
    {
        public Contract(string userName, string oppositeUserName)
        {
            UserName = userName;
            OppositeUserName = oppositeUserName;
        }

        public string OppositeUserName { get; private set; }

        public string UserName { get; private set; }

        public override bool Equals(object obj)
        {
            var contract = obj as Contract;
            if (contract == null)
            {
                return false;
            }

            if (ReferenceEquals(this, contract))
            {
                return true;
            }

            return UserName == contract.UserName && OppositeUserName == contract.OppositeUserName;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = -302213817;
                var multiplier = -1521134295;

                hashCode = (hashCode * multiplier) + UserName.GetHashCode();
                hashCode = (hashCode * multiplier) + OppositeUserName.GetHashCode();

                return hashCode;
            }
        }

        public override string ToString() => UserName + OppositeUserName;
    }
}