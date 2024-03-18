using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Component = UnityEngine.Component;

namespace IziHardGames.Libs.Engine.Helpers
{
	public struct EnumerableForComponentsAtScene<T> : IEnumerable<T> where T : Component
	{
		private Scene scene;

		public EnumerableForComponentsAtScene(Scene scene)
		{
			this.scene = scene;
		}
		public EnumeratorForComponentsAtScene<T> GetEnumerator()
		{
			return new EnumeratorForComponentsAtScene<T>(scene);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
#if DEBUG
			throw new Exception($"Boxing occured");
#endif
			return new EnumeratorForComponentsAtScene<T>(scene);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
#if DEBUG
			throw new Exception($"Boxing occured");
#endif
			return new EnumeratorForComponentsAtScene<T>(scene);
		}
	}

	public struct EnumeratorForComponentsAtScene<T> : IEnumerator<T> where T : Component
	{
		object IEnumerator.Current { get => throw new System.NotSupportedException(); }
		public T Current { get; set; }

		private T[] components;
		private int indexGoRoot;
		private int indexGoChild;
		private int indexComponent;
		private bool isReachEnd;

		private GameObject currentGo;
		private GameObject[] roots;

		private Transform parent;
		private Scene scene;

		public EnumeratorForComponentsAtScene(Scene scene)
		{
			this.scene = scene;
			roots = scene.GetRootGameObjects();
			currentGo = null;

			components = default;
			Current = default;
			parent = default;
			indexGoRoot = -1;
			indexComponent = -1;
			indexGoChild = default;
			isReachEnd = false;
		}

		public bool MoveNext()
		{
			if (roots.Length > 0)
			{
				// устанавливаем в самый первый раз геймобжект
				if (currentGo == null)
				{
					parent = null;
					indexGoRoot = 0;

					SetGameObject(roots.First());
				}
				else
				{
					indexComponent++;
				}

				REPEAT:
				{
					// проверяем можно ли перейти на следующий компонент
					if (indexComponent < components.Length)
					{
						Current = components[indexComponent];
						return true;
					}
					else
					{
						if (!isReachEnd)
						{
							NextGameObject();
							goto REPEAT;
						}
					}
				}
			}
			return false;
		}

		private void NextGameObject()
		{
			// пытаемся войти в глубь иерархии
			if (currentGo.transform.childCount > 0)
			{
				parent = currentGo.transform;
				SetGameObject(parent.GetChild(0).gameObject);
				indexGoChild = 0;
				return;
			}

			REPEAT:
			// иначе проверяем можно ли двигаться в пределах сиблингов
			if (parent != null)
			{
				indexGoChild++;
				if (indexGoChild < parent.childCount)
				{
					SetGameObject(parent.GetChild(indexGoChild).gameObject);
					return;
				}
				else
				{
					// закончилось движение в пределах сиблингов. Возвращаемся на уровень вверх.
					indexGoChild = parent.GetSiblingIndex();
					parent = parent.parent;
					goto REPEAT;
				}
			}
			else
			{
				indexGoRoot++;
				if (indexGoRoot < roots.Length)
				{
					SetGameObject(roots[indexGoRoot]);
				}
				else
				{
					isReachEnd = true;
					return;
				}
			}
		}

		private void SetGameObject(GameObject go)
		{
			currentGo = go;
			components = currentGo.GetComponents<T>();
			indexComponent = 0;
		}

		public void Reset()
		{
			this = new EnumeratorForComponentsAtScene<T>(scene);
		}


		public void Dispose()
		{
			this = default;
		}
	}
}