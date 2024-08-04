using System.Collections;
using UnityEngine;

namespace Code.CoroutineRunner
{
  public interface ICoroutineRunner
  {
    public Coroutine StartCoroutine(IEnumerator routine);
  }
}