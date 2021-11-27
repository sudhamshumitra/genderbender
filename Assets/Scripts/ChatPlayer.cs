using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChatElement
{
   [SerializeField]
   private float waitBeforeSending;
   [SerializeField]
   private float waitWhileTyping;
   [SerializeField]
   private string chatContent;
   [SerializeField] 
   private bool isUserOwnedChat;

   public bool GetIsUserOwnedChat() => isUserOwnedChat;
   public string GetContent() => chatContent;
   public float GetTypingWait() => waitWhileTyping;
   public float GetWaitBeforeNextChat() => waitBeforeSending;
}

[System.Serializable]
public class ChatData
{
   [SerializeField]
   private List<ChatElement> ChatElements;
   [SerializeField]
   private int currentElement = -1;
   
   public void OnComplete()
   {
      Debug.Log($"Called on complete for this chatData {this}");
   }
   public ChatElement GetNextElement()
   {
      currentElement++;
      if (currentElement > ChatElements.Count - 1)
      {
         OnComplete();
         return null;
      }
      return ChatElements[currentElement];
   }
}

public class ChatPlayer : MonoBehaviour
{
   [SerializeField]
   private AudioSource messageReceivedAudioSource;

   [SerializeField] private float soundPreponeTime = 0.3f;
   public void PlayMessageReceivedAudio()
   {
      messageReceivedAudioSource.Play();
   }
   
   [SerializeField] private GameObject typingGameObject;
   [SerializeField] private ChatData _chatData;
   [SerializeField] private ChatWindow _chatWindow;
   [SerializeField] private bool isChatBeingPlayed = false;

   private void Start()
   {
      Play(_chatData);
   }

   private void Play(ChatData chatData)
   {
      _chatData = chatData;
      isChatBeingPlayed = true;
      StartCoroutine(ChatEnumerator());
   }

   private void ShowTyping(ChatElement chatElement)
   {
      StartCoroutine(RunTypingEnumerator(chatElement));
   }
   
   private IEnumerator RunTypingEnumerator(ChatElement chatElement)
   {
      var chatRect = typingGameObject.GetComponent<RectTransform>();
      chatRect.pivot = chatElement.GetIsUserOwnedChat() ? new Vector2(1, 1) : new Vector2(0, 1);
      var chatNewPosition = _chatWindow.GetNewElementPosition(chatElement.GetIsUserOwnedChat());
      _chatWindow.SetProceduralUICorners(chatElement.GetIsUserOwnedChat(), typingGameObject.GetComponent<FreeModifier>());
      
      var position = chatRect.position;
      position.Set(chatNewPosition.x, chatNewPosition.y, position.z);
      chatRect.anchoredPosition = position;
      
      typingGameObject.SetActive(true);
      yield return new WaitForSeconds(chatElement.GetTypingWait());
      typingGameObject.SetActive(false);
   }
   
   private IEnumerator ChatEnumerator()
   {
      var nextChatElement = _chatData.GetNextElement();
      if (nextChatElement == null) yield break;
      
      ShowTyping(nextChatElement);

      yield return new WaitForSeconds(nextChatElement.GetWaitBeforeNextChat() - soundPreponeTime);
      if(!nextChatElement.GetIsUserOwnedChat()) PlayMessageReceivedAudio();
      yield return new WaitForSeconds(soundPreponeTime);
      
      _chatWindow.PlayElement(nextChatElement,typingGameObject);
      StartCoroutine(ChatEnumerator());
   }
}
