// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Cards.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Protobuf.Cards {

  /// <summary>Holder for reflection information generated from Cards.proto</summary>
  public static partial class CardsReflection {

    #region Descriptor
    /// <summary>File descriptor for Cards.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CardsReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgtDYXJkcy5wcm90bxIac3luZXJneUJsdWUucHJvdG9idWYuY2FyZHMiHQoL",
            "UGxheWluZ0NhcmQSDgoGQ2FyZElkGAEgASgFKjgKBUVTdWl0EgoKBlNwYWRl",
            "cxAAEgkKBUNsdWJzEAESCgoGSGVhcnRzEAISDAoIRGlhbW9uZHMQAyqWAQoF",
            "RVJhbmsSDAoIX1Vua25vd24QABIGCgJfMhABEgYKAl8zEAISBgoCXzQQAxIG",
            "CgJfNRAEEgYKAl82EAUSBgoCXzcQBhIGCgJfOBAHEgYKAl85EAgSBwoDXzEw",
            "EAkSCQoFX0phY2sQChIKCgZfUXVlZW4QCxIJCgVfS2luZxAMEggKBF9BY2UQ",
            "DRIKCgZfSm9rZXIQDmIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Protobuf.Cards.ESuit), typeof(global::Protobuf.Cards.ERank), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Protobuf.Cards.PlayingCard), global::Protobuf.Cards.PlayingCard.Parser, new[]{ "CardId" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  /// <summary>
  /// The ordering of this Enum may be important in mapping card images to PlayingCards. Do not change.
  /// </summary>
  public enum ESuit {
    [pbr::OriginalName("Spades")] Spades = 0,
    [pbr::OriginalName("Clubs")] Clubs = 1,
    [pbr::OriginalName("Hearts")] Hearts = 2,
    [pbr::OriginalName("Diamonds")] Diamonds = 3,
  }

  /// <summary>
  /// The ordering of this Enum may be important in mapping card images to PlayingCards. Do not change.
  /// </summary>
  public enum ERank {
    [pbr::OriginalName("_Unknown")] Unknown = 0,
    [pbr::OriginalName("_2")] _2 = 1,
    [pbr::OriginalName("_3")] _3 = 2,
    [pbr::OriginalName("_4")] _4 = 3,
    [pbr::OriginalName("_5")] _5 = 4,
    [pbr::OriginalName("_6")] _6 = 5,
    [pbr::OriginalName("_7")] _7 = 6,
    [pbr::OriginalName("_8")] _8 = 7,
    [pbr::OriginalName("_9")] _9 = 8,
    [pbr::OriginalName("_10")] _10 = 9,
    [pbr::OriginalName("_Jack")] Jack = 10,
    [pbr::OriginalName("_Queen")] Queen = 11,
    [pbr::OriginalName("_King")] King = 12,
    [pbr::OriginalName("_Ace")] Ace = 13,
    [pbr::OriginalName("_Joker")] Joker = 14,
  }

  #endregion

  #region Messages
  public sealed partial class PlayingCard : pb::IMessage<PlayingCard> {
    private static readonly pb::MessageParser<PlayingCard> _parser = new pb::MessageParser<PlayingCard>(() => new PlayingCard());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<PlayingCard> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Protobuf.Cards.CardsReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PlayingCard() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PlayingCard(PlayingCard other) : this() {
      cardId_ = other.cardId_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PlayingCard Clone() {
      return new PlayingCard(this);
    }

    /// <summary>Field number for the "CardId" field.</summary>
    public const int CardIdFieldNumber = 1;
    private int cardId_;
    /// <summary>
    /// Internal Representation. Should not use in code- Maybe use CardIndex instead.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CardId {
      get { return cardId_; }
      set {
        cardId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as PlayingCard);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(PlayingCard other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (CardId != other.CardId) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (CardId != 0) hash ^= CardId.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (CardId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(CardId);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (CardId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(CardId);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(PlayingCard other) {
      if (other == null) {
        return;
      }
      if (other.CardId != 0) {
        CardId = other.CardId;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            CardId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
