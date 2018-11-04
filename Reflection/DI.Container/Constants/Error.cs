namespace DI.Container.Constants
{
    public static class Error
    {
        public const string ParameterWasAlreadyRegistred = "Parameter {0} was already registred.";
        public const string KeyWasAlreadyRegistred = "Key \"{0}\" was already registred.";
        public const string NoRegisteredTypeForInterface = "No registred type for interface {0}";
        public const string KeyInvalidOrDoesntExist = "Key : \"{0}\" doesn't exist or invalid.";
        public const string NoInheritanceBetweenTypes = "No inheritance between {0} and {1}";
        public const string NoMatchingCtorsForType = "No matching ctors for type {0}";
        public const string CyclingReferenceDetected = "Cycling reference was detected.";
        public const string MoreThanOneContructorsMatches = "Don't know what constructor to choose.";
    }
}