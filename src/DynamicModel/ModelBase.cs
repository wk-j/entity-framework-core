using System.ComponentModel.DataAnnotations;

namespace DynamicModel {
    public class ModelBase {
        [Key]
        public int __Id__ { set; get; } = 1;
    }
}
