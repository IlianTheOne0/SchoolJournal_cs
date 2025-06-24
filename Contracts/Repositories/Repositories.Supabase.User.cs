namespace Contracts.Repositories.Supabase;

using Contracts.Models.Tables.Users;
using System.Threading.Tasks;

public partial class RepositoriesSupabase
{
    public async Task<ModelsUser?> GetUserById(int UserId)
    {
        try
        {
            var result = await FilterAsync<ModelsUser>("Id", global::Supabase.Postgrest.Constants.Operator.Equals, UserId);
            return result?.FirstOrDefault();
        }
        catch (Exception e) { throw new Exception($"Failed to get user by ID: {e.Message}", e); }
    }

    public async Task UpdateUser(ModelsUser User)
    {
        try
        {
            var existingUserResult = await SupabaseConnection?.SupabaseClient
                .From<ModelsUser>()
                .Where(u => u.Id == User.Id)
                .Get()!;

            var existingUser = existingUserResult.Models.FirstOrDefault();
            if (existingUser == null) { throw new ArgumentException("User not found!"); }

            if (!string.Equals(existingUser.Username, User.Username, StringComparison.OrdinalIgnoreCase))
            {
                var usernameExistsResult = await SupabaseConnection.SupabaseClient
                    .From<ModelsUser>()
                    .Where(u => u.Username == User.Username && u.Id != User.Id)
                    .Get();

                if (usernameExistsResult.Models.Count > 0) { throw new ArgumentException("Username already exists!"); }
            }

            if (!string.Equals(existingUser.PhoneNumber, User.PhoneNumber, StringComparison.OrdinalIgnoreCase))
            {
                if (User.PhoneNumber?.Length != 10) { throw new ArgumentException("Phone number must be exactly 10 characters long!"); }

                var phoneNumberExistsResult = await SupabaseConnection.SupabaseClient
                    .From<ModelsUser>()
                    .Where(u => u.PhoneNumber == User.PhoneNumber && u.Id != User.Id)
                    .Get();

                if (phoneNumberExistsResult.Models.Count > 0) { throw new ArgumentException("Phone number already exists!"); }
            }

            if (User.DateOfBirth > DateTime.Now) { throw new ArgumentException("Date of birth cannot be in the future!"); }

            var response = await SupabaseConnection.SupabaseClient
                .From<ModelsUser>()
                .Where(userProvider => userProvider.Id == User.Id)
                .Set(userProvider => userProvider.FullName, User.FullName)
                .Set(userProvider => userProvider.Username, User.Username)
                .Set(userProvider => userProvider.PhoneNumber, User.PhoneNumber)
                .Set(userProvider => userProvider.Sex, User.Sex)
                .Set(userProvider => userProvider.DateOfBirth, User.DateOfBirth)
                .Set(userProvider => userProvider.DateOfTheLastUpdate, User.DateOfTheLastUpdate)
                .Set(userProvider => userProvider.DateOfTheLastVisitToTheJournal, User.DateOfTheLastVisitToTheJournal)
                .Set(userProvider => userProvider.AvatarUrl!, User.AvatarUrl)
                .Update();

            if (response.ResponseMessage?.IsSuccessStatusCode != true) { throw new Exception("Failed to update user in database"); }
        }
        catch (Exception e) { throw new Exception($"Failed to update user: {e.Message}", e); }
    }

    public async Task UpdateAvatar(int UserId, string FilePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(FilePath)) { throw new ArgumentException("File path cannot be empty"); }

            if (!File.Exists(FilePath)) { throw new FileNotFoundException("Selected file does not exist", FilePath); }

            var bucket = SupabaseConnection?.SupabaseClient.Storage.From("avatars");
            if (bucket == null) { throw new Exception("Failed to access avatars bucket"); }

            var fileExtension = Path.GetExtension(FilePath).ToLower();
            if (string.IsNullOrEmpty(fileExtension)) { fileExtension = ".jpg"; }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(fileExtension)) { throw new ArgumentException("Only JPG, JPEG, and PNG files are allowed"); }

            var fileName = $"avatar_{UserId}_{DateTime.Now.Ticks}{fileExtension}";

            byte[] fileBytes = await File.ReadAllBytesAsync(FilePath);

            await bucket.Upload(
                fileBytes,
                fileName,
                new global::Supabase.Storage.FileOptions
                {
                    ContentType = GetContentType(fileExtension)
                }
            );

            var url = bucket.GetPublicUrl(fileName);

            await SupabaseConnection?.SupabaseClient
                .From<ModelsUser>()
                .Where(user => user.Id == UserId)
                .Set(user => user.AvatarUrl!, url)
                .Update()!;
        }
        catch (Exception e) { throw new Exception($"Update avatar failed: {e.Message}", e); }
    }

    private string GetContentType(string fileExtension)
    {
        return fileExtension switch
        {
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "application/octet-stream"
        };
    }
}