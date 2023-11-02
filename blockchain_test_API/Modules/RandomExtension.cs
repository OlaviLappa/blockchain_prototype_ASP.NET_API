namespace blockchain_test_API.Modules
{
    public static class RandomExtension
    {
        public static int[] GenerateSeedNumbersRandom(this Random random, int fixLength)
        {
            int min = 0;
            int max = 2048;

            int[] randomSequence = new int[fixLength];

            for (int i = 0; i < randomSequence.Length; i++)
            {
                int number = random.Next(min, max) + 1;
                randomSequence[i] = number;
            }

            return randomSequence;
        }

        public static List<string> GenerateSeedPhraseRandom(this Random random, int[] randomSequence)
        {
            string filePath = "Data/seed-phrase-russian.txt";

            List<string> seedPhrase = new List<string>();
            string[] allWords = new string[2049];

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    for (int i = 0; i < allWords.Length; i++)
                    {
                        allWords[i] = reader.ReadLine();
                    }
                }

                using (StreamReader reader = new StreamReader(filePath))
                {
                    for (int i = 0; i < randomSequence.Length; i++)
                    {
                        for (int j = 0; j <= randomSequence[i]; j++)
                        {
                            if (j == randomSequence[i])
                            {
                                seedPhrase.Add(allWords[j]);

                                break;
                            }
                        }
                    }
                }

                return seedPhrase;
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}