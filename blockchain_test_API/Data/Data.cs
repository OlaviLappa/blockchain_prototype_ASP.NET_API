using System.IO;

namespace blockchain_test_API.Data
{
    public class Data
    {
        public string[] GetAllPublicKeys()
        {
            string[] allPublicKeys = new string[10000];

            using (StreamReader reader = new StreamReader("Data/public-keys.txt"))
            {
                for (int i = 0; i < allPublicKeys.Length; i++)
                {
                    allPublicKeys[i] = reader.ReadLine();
                }
            }

            return allPublicKeys;
        }
    }
}