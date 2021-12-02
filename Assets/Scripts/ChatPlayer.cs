using System;
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
   public ChatData(List<ChatElement> chatElements)
   {
      this.ChatElements = chatElements;
      Reset();
   }
   
   [SerializeField]
   private List<ChatElement> ChatElements;
   [SerializeField]
   private int currentElement = -1;

   public void Reset()
   {
      currentElement = -1;
   }
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
   [SerializeField] private AudioClip sentSFX;
   [SerializeField] private AudioClip receivedSFX;
   [SerializeField] private AudioClip typingSFX;
   
   [SerializeField] private AudioSource sentSource;
   [SerializeField] private AudioSource receivedSource;
   [SerializeField] private AudioSource typingSource;

   public void PlaySent()
   {
      sentSource.Play();
      messageReceivedAudioSource.clip = sentSFX;
      messageReceivedAudioSource.Play();
   }
   
   public void PlayReceived()
   {
      receivedSource.Play();

      messageReceivedAudioSource.clip = receivedSFX;
      messageReceivedAudioSource.Play();
   }

   public IEnumerator TypingRoutine(float typingWait)
   {
      PlayTyping(true);
      yield return new WaitForSeconds(typingWait);
      PlayTyping(false);
   }
   public void PlayTyping(bool play)
   {
      if (play)
      {
         
         typingSource.Play();

         messageReceivedAudioSource.clip = typingSFX;
         messageReceivedAudioSource.Play();         
      }
      else
      {
         typingSource.Stop();

         messageReceivedAudioSource.Stop();
      }
   }
   
   
   [SerializeField]
   private AudioSource messageReceivedAudioSource;

   [SerializeField] private float soundPreponeTime = 0.3f;
   
   [SerializeField] private GameObject typingGameObject;
   [SerializeField] public ChatData _chatData;
   [SerializeField] private ChatWindow _chatWindow;

   private Coroutine chatRoutine;

   private void OnDisable()
   {
      _chatData.Reset();
      if (chatRoutine != null) StopCoroutine(chatRoutine);
   }

   private void OnEnable()
   {
      Play(_chatData);
   }

   private void Play(ChatData chatData)
   {
      _chatData = chatData;
      chatRoutine = StartCoroutine(ChatEnumerator());
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
      PlayTyping(true);
      yield return new WaitForSeconds(chatElement.GetTypingWait());
      typingGameObject.SetActive(false);
      PlayTyping(false);
   }
   
   private IEnumerator ChatEnumerator()
   {
      var nextChatElement = _chatData.GetNextElement();
      if (nextChatElement == null) yield break;
      
      ShowTyping(nextChatElement);

      yield return new WaitForSeconds(nextChatElement.GetWaitBeforeNextChat() - soundPreponeTime);
      if (!nextChatElement.GetIsUserOwnedChat())
      {
         PlayReceived();
      }
      else
      {
         PlaySent();
      }
      yield return new WaitForSeconds(soundPreponeTime);
      
      _chatWindow.PlayElement(nextChatElement,typingGameObject);
      chatRoutine = StartCoroutine(ChatEnumerator());
   }
}
