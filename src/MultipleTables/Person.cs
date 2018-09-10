using System.ComponentModel.DataAnnotations;

namespace MultipleTables {
    class Person {
        [Key]
        public int Id { set; get; }

        public string Name { set; get; }
        public string LastName { set; get; }
    }
}
