using Newtonsoft.Json;

namespace Models
{
    public class FullName
    {
        [JsonProperty("first")]
        public string FirstName { get; set; }
        [JsonProperty("last")]
        public string LastName { get; set; }
        [JsonProperty("mid")]
        public string MidName { get; set; }

        public FullName() { }

        public FullName(string fullName)
        {
            var data = fullName?.Split(' ');
            FirstName = data[data.Length - 1];
            LastName = data[0];
            var mid = "";
            for (int i = 1; i < data.Length - 1; i++)
            {
                mid += data[i] + " ";
            }
            MidName = mid.TrimEnd();
        }

        public FullName(string firstName, string lastName, string midName)
        {
            FirstName = firstName;
            LastName = lastName;
            MidName = midName;
        }

        public override string ToString()
        {
            return $"{LastName} {MidName} {FirstName}";
        }
    }
}
