namespace SendPulseNetSDK.SendPulse.Models;


public class EmailDetails
{
    public string id { get; set; }
    public string sender { get; set; }
    public int total_size { get; set; }
    public string sender_ip { get; set; }
    public int smtp_answer_code { get; set; }
    public string smtp_answer_subcode { get; set; }
    public string smtp_answer_data { get; set; }
    public string used_ip { get; set; }
    public object recipient { get; set; }
    public string subject { get; set; }
    public string send_date { get; set; }
    public Tracking tracking { get; set; }
}

public class Tracking
{
    public int click { get; set; }
    public int open { get; set; }
    public Link[] link { get; set; }
    public Client_Info[] client_info { get; set; }
}

public class Link
{
    public string url { get; set; }
    public string browser { get; set; }
    public string os { get; set; }
    public string screen_resolution { get; set; }
    public string ip { get; set; }
    public string country { get; set; }
    public string action_date { get; set; }
}

public class Client_Info
{
    public string browser { get; set; }
    public string os { get; set; }
    public string ip { get; set; }
    public string country { get; set; }
    public string action_date { get; set; }
}