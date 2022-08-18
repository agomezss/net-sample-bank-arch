using Bank.Core.Models;

namespace Bank.Core.ViewModels
{
  public class PostAuthenticationResponse {

    public bool IsAuthenticated { get; set; }
    public Customer Customer { get; set; }
  }
}
