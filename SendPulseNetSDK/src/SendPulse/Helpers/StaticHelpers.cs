namespace SendPulseNetSDK.src.SendPulse.Helpers;

public class StaticHelpers
{
    public static Dictionary<string, string> ConvertAttachmentsToBase64(Dictionary<string, byte[]> attachments)
    {
        return attachments.ToDictionary(
            attachment => attachment.Key,
            attachment => Convert.ToBase64String(attachment.Value)
        );
    }

}