using System;

namespace Bank.Core.ViewModels
{
  public class ActivityLogBaseRequest : BaseRequest
  {
    public string ActivityLogId { get; set; }

    public string Channel { get; set; }

    public DateTime TransactionTime { get; set; }

    public string Origin { get; set; }

    public string Category { get; set; }

    public string ActivityJsonData { get; set; }

    public ActivityLogBaseRequest()
    {
      ActivityLogId = Guid.NewGuid().ToString();
      Origin = "Bank";
      Channel = "app";
      TransactionTime = DateTime.Now;
    }
  }
}
