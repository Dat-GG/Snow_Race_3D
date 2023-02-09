// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("RVikA2ZaH3f2beAom41oMZ6gQoGl5jrLgPfjYcorzJ0UoC8ljJfPQ3f0+vXFd/T/93f09PV3SIs8Q5e5YXZZl0oXP7E7Kcv375zbG84CUA1SnECTc3cg4AiGoUAeF2bNZl9XYLoXKSX0r0vGD/HEgqJ9bOHZf5a9CtnvHzf2bCTrjhbZVNYCkMYWParFd/TXxfjz/N9zvXMC+PT09PD19opleXanQD0QYu1w+ctMfKlzwGQwWYjC0kGqEorst4R+6tIUmm8mjOXfZFIUxQvbCtlfhy93SIN5xiis6A4Oqk+9IEYphRkSYgsiIazsObihq79OnoonCIwNxMi36hrCP14SbI9Hw3gl7rcC7abkX+HKmjbCRNnc8HYPEpQv2bPGvvf29PX0");
        private static int[] order = new int[] { 4,10,10,12,10,6,6,10,9,13,11,13,12,13,14 };
        private static int key = 245;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
