﻿using System.Text.Json.Serialization;

namespace SendPulseNetSDK.SendPulse.Models;

public class EmailResponse
{
    [JsonPropertyName("result")]
    public bool Result { get; set; }

    [JsonPropertyName("id")] 
    public string Id { get; set; } = null!;
}