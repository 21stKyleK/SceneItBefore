[HttpPost("exchange-token")]
public async Task<IActionResult> ExchangeToken([FromBody] string code)
{
    var client = new HttpClient();
    var request = new HttpRequestMessage(HttpMethod.Post, "https://YOUR_AUTH0_DOMAIN/oauth/token");

    // Adding the Auth0 authorization params required for the token exchange
    var content = new FormUrlEncodedContent(new[]
    {
        new KeyValuePair<string, string>("client_id", "eNIZK1CtP2TTXt6NUGF6AW4ghRfx6VKA"),
        new KeyValuePair<string, string>("client_secret", "t-xOACUcdqd3k-90zTGBxxzzkMi4slc0Ia9aZuPeDwxbcr_MJWDH6KsHV7gGB5uo"),
        new KeyValuePair<string, string>("code", code),
        new KeyValuePair<string, string>("grant_type", "authorization_code"),
        new KeyValuePair<string, string>("redirect_uri", "https://www.SceneItBefore.org/accountsMainPage.html")
    });
    request.Content = content;

    // Send the request and handle the response
    var response = await client.SendAsync(request);
    if (response.IsSuccessStatusCode)
    {
        var responseBody = await response.Content.ReadAsStringAsync();
        // Then the tokens get sent back to the requesting client
        return Ok(responseBody);
    }

    return BadRequest("Failed to exchange token");
}