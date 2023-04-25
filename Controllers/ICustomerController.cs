using Models;

namespace Controllers
{
    public interface ICustomerController
    {
        bool IsNameValid(string name);
        bool IsEmailValid(string email);
        bool IsPhoneValid(string phone);
        int CompareCustomerById(Customer customer1, Customer customer2);
        int CompareCustomerByName(Customer customer1, Customer customer2);
        int CompareCustomerByBirthDate(Customer customer1, Customer customer2);
        int CompareCustomerByCreatedDate(Customer customer1, Customer customer2);
        int CompareCustomerByPointDESC(Customer customer1, Customer customer2);
        bool IsCustomerNameMatch(Customer customer, string name);
        bool IsCustomerAddressMatch(Customer customer, string address);
        bool IsCustomerIdMatch(Customer customer, string id);
        bool IsCustomerTypeMatch(Customer customer, string type);
        bool IsCustomerPhoneNumberMatch(Customer customer, string phoneNumber);
    }
}
