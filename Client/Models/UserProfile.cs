using System;

namespace UI.Models
{
    public class UserProfile : IComparable<UserProfile> {

        public string Email { get; set; } = "abc";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Town { get; set; }
        public string Password { get; set; }
        public DateTime BirthDay { get; set; }
        public Gender Gender { get; set; } = Gender.Female;
        public Guid user_id { get; private set; } = new Guid();

        public int CompareTo(UserProfile other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var emailComparison = string.Compare(Email, other.Email, StringComparison.Ordinal);
            if (emailComparison != 0) return emailComparison;
            var firstNameComparison = string.Compare(FirstName, other.FirstName, StringComparison.Ordinal);
            if (firstNameComparison != 0) return firstNameComparison;
            var lastNameComparison = string.Compare(LastName, other.LastName, StringComparison.Ordinal);
            if (lastNameComparison != 0) return lastNameComparison;
            var townComparison = string.Compare(Town, other.Town, StringComparison.Ordinal);
            if (townComparison != 0) return townComparison;
            var passwordComparison = string.Compare(Password, other.Password, StringComparison.Ordinal);
            if (passwordComparison != 0) return passwordComparison;
            var birthDayComparison = BirthDay.CompareTo(other.BirthDay);
            if (birthDayComparison != 0) return birthDayComparison;
            var genderComparison = Gender.CompareTo(other.Gender);
            if (genderComparison != 0) return genderComparison;
            return user_id.CompareTo(other.user_id);
        }
        
    }
}
