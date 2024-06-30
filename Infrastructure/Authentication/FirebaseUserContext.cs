//using Firebase.Auth;
//using PlogPal.Application.Common.Interfaces;

//namespace Infrastructure.Authentication;

//public class FirebaseUserContext : IUserContext
//{
//    private UserCredential _userCredential;

//    public string UserId => _userCredential.User.Uid;
//    public string Name => _userCredential.User.Info.DisplayName;
//    public string BearerToken => _userCredential.User.Credential.IdToken;


//    public FirebaseUserContext(UserCredential userCredential)
//    {
//        _userCredential = userCredential;
//    }

//    public void SetUserContext(IUserContext userContext)
//    {

//    }
//}
