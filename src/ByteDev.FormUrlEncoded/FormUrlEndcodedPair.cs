namespace ByteDev.FormUrlEncoded
{
    internal class FormUrlEndcodedPair
    {
        private readonly string[] _pairArray;
        private readonly DeserializeOptions _options;

        private string _name;
        private string _value;
        
        public bool IsValid => _pairArray.Length == 2 && _pairArray[1] != string.Empty;

        public string Name => _name ?? (_name = UrlEncoder.Decode(_pairArray[0], _options));

        public string Value => _value ?? (_value = UrlEncoder.Decode(_pairArray[1], _options));

        public FormUrlEndcodedPair(string pair, DeserializeOptions options)
        {
            _pairArray = pair.Split('=');
            _options = options;
        }
    }
}