namespace Safe_Sign.Util
{
    public class FieldsValidationTools
    {
        #region CPF
        private static int VerifyCPFDigits(string CPF, int max)
        {
            int count = 0;
            foreach (char digit in CPF)
            {
                count += int.Parse(digit.ToString()) * max;

                max--;

                if (max < 2) {
                    int rest = (count * 10) % 11;
                    
                    return rest == 10 ? 0 : rest;
                };
            }

            return count;
        }

        private static bool AreDigitsDifferents(string CPF)
        {
            char firstDigit = CPF[0];
            foreach (char digit in CPF)
            {
                if (digit != firstDigit) return true;
            }

            return false;
        }

        public static bool ValidationCPF(string CPF)
        {
            int firstValidatorDigit = VerifyCPFDigits(CPF, 10);
            int secondValidatorDigit = VerifyCPFDigits(CPF, 11);

            if (AreDigitsDifferents(CPF))
            {
                return (int.Parse(CPF[9].ToString()) == firstValidatorDigit) && (int.Parse(CPF[10].ToString()) == secondValidatorDigit); //Verifico se o resto que recebo em cada verificação, é correspondente ao digito verificador respectivo
            }

            return false;
        }
        #endregion

        #region CNPJ
        private static int VerifyCPNJFirstDigit(string CNPJ)
        {
            int[] sequence = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int count = 0;
            for (int i = 0; i < 12; i++)
            {
                count += int.Parse(CNPJ[i].ToString()) * sequence[i];
            }

            int quotient = count % 11;

            if (quotient < 2) return 0;

            else return 11 - quotient;
        }

        private static int VerifyCNPJSecondDigit(string CNPJ)
        {
            int[] sequence = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int count = 0;
            for (int i = 0; i < 13; i++)
            {
                count += int.Parse(CNPJ[i].ToString()) * sequence[i];
            }

            int quotient = count % 11;
            
            if (quotient < 2) return 0;

            else return 11 - quotient;
        }

        public static bool ValidationCNPJ(string CNPJ)
        {
            int firstDigit = VerifyCPNJFirstDigit(CNPJ);
            int secondDigit = VerifyCNPJSecondDigit(CNPJ);
                       
            // Foreach one of already checked pieces, check if are equal to the validator
            return (int.Parse(CNPJ[12].ToString()) == firstDigit) && (int.Parse(CNPJ[12].ToString()) == secondDigit);
        }
        #endregion
    }
}