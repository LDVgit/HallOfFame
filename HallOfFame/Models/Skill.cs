using Newtonsoft.Json;

namespace HallOfFame.Models
{
    public class Skill
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string Name { get; set; }
        public byte Level { get; set; }
        [JsonIgnore]
        public long PersonId { get; set; }
        public Person Person { get; set; }
    }
}
