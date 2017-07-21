using System.ComponentModel.DataAnnotations;

namespace SwappableImplementation
{
    public class Light
    {
        [Key]
        public string Key { get; set; }
        public string Name { get; set; }
        public LightState State { get; set; }
    }
}
