namespace OneTrack.PM.Entities.DTOs.Security
{
    public class Tokens
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string FireBaseToken { get; set; }
    }
    public struct TokenValidation
    {
        public string AccessToken { get; set; }
    }
    public struct TokenClaims
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int Code { get; set; }
        public string Barcode { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public int MainModuleId { get; set; }
        public string Email { get; set; }
        public string FireBaseToken { get; set; }
    }
}
