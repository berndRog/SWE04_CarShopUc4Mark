namespace CarShop; 
public abstract class Result<T> where T : class? {
   public string Message { get; }
   public T?     Data    { get; }

   protected Result(
      string message = "",
      T?     data    = null
   ) {
      Message = message;
      Data    = data;
   }
}

public class Success<T> : Result<T> where T : class? {
   public Success(T? data) : base("", data) { }
}

public class Error<T> : Result<T> where T : class? {
   public Error(string error) : base(error) { }
}

public class Loading<T> : Result<T> where T : class? {
   public Loading(string message) : base(message) { }
}


/*
sealed class Resource <T>(
val message: String = "",
val data:    T?      = null
) {
class Success<T>(data:         T     ): Resource<T>("", data)
class Error<T>  (errorMessage: String): Resource<T>(errorMessage, null)
class Loading<T>(message     : String): Resource<T>(message, null)
}

val Resource<*>.succeeded: Boolean
get() = this is Resource.Success <*> && data != null


fun <T> handleResource(
resource: Resource<T> ,
                      liveState: MutableLiveData<Resource<String>> , 
case: String
): T? {
when(resource) {
   is Resource.Loading ->
         liveState.postValue(Resource.Loading("$case person"))
      is Resource.Success -> {
      liveState.postValue(Resource.Success("$case person successfully"))
      return resource.data
   }
   is Resource.Error ->
      liveState.postValue(Resource.Success(resource.message))
}
return null
}

*/