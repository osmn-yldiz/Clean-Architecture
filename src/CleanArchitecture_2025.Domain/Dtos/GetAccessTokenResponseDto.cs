using System.Text.Json.Serialization;

namespace CleanArchitecture_2025.Domain.Dtos;

public sealed class GetAccessTokenResponseDto
{
    [JsonPropertyName("Token")]
    public string Token { get; set; } = default!;
}
