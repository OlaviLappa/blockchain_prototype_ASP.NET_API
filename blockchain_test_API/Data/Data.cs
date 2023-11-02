using System.IO;

namespace blockchain_test_API.Data
{
    public class Data
    {
        public string[] GetAllPublicKeys()
        {
            string[] allPublicKeys = new string[1000];

            using (StreamReader reader = new StreamReader("Data/public-keys.txt"))
            {
                for (int i = 0; i < allPublicKeys.Length; i++)
                {
                    allPublicKeys[i] = reader.ReadLine();
                }
            }

            return allPublicKeys;
        }

        public async Task<string[]> GetAllPublicKeysAsync()
        {
            string[] allPublicKeys;

            try
            {
                allPublicKeys = await File.ReadAllLinesAsync("Data/public-keys.txt");
            }

            catch (Exception ex)
            {
                throw new Exception($"Error at file reading: {ex.Message}");
            }

            return allPublicKeys;
        }
    }
}