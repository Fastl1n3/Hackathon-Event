using Contracts;

namespace HRDirector;

public class MessageAccumulator {
    private readonly List<WishlistRequest> _messages = new();
    
    public void AddMessage(WishlistRequest message) {
        _messages.Add(message);
    }
    
    public List<WishlistRequest> GetMessages() {
        return _messages;
    }
    
    public void ClearMessages() {
        _messages.Clear();
    }
}