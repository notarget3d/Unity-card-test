using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;


namespace SharedCode
{
	public class ProjectUtils : MonoBehaviour
	{
		public static async Task<Texture2D> GetRandomTexture(Vector2Int size)
		{
			return await GetRandomTexture(size.x, size.y);
		}

		public static async Task<Texture2D> GetRandomTexture(int width, int height)
		{
			string url = $"https://picsum.photos/{width}/{height}";
			var uwr = UnityWebRequestTexture.GetTexture(url);

			var t = uwr.SendWebRequest();

			while (t.isDone == false)
			{
				await Task.Yield();
			}

			if (uwr.result == UnityWebRequest.Result.Success)
			{
				return ((DownloadHandlerTexture)uwr.downloadHandler).texture;
			}
			else
			{
				Debug.LogError($"Error! GetRandomTexture failed, result: {uwr.result}");
				return null;
			}
		}

		// Just for fun pick a random card title
		private static int _idx = -1;
		private static readonly string[] NAMES = new string[]
		{
			"Davon", "Alexus", "Isabel", "Yareli", "Ethan", "Rodolfo", "Caden", "Quincy", "Eddie", "Ashley", "Lance",
			"Kale", "Junior", "Davion", "Collin", "Darius", "Arturo", "Tyler", "Albert", "Ciara", "Luciano",
			"Lilianna", "Geovanni", "Kiley", "Arianna", "Moriah", "Bailee", "Conner", "Kassandra", "Mara",
			"Mccormick", "Ball", "Durham", "Tyler", "Warner", "Russell", "Barrera", "Stevenson", "Goodwin", "Poole",
			"Olson", "Cowan", "Nichols", "Mccann", "Archer", "Ramsey", "Stein", "Hart", "Roberson", "Estes",
			"Snyder", "Ayers", "Mccann", "Levine", "Hall", "Jarvis", "Davis", "Frey", "Caldwell", "Gordon"
		};


		public static string GetRandomName()
		{
			if (_idx == -1)
			{
				_idx = UnityEngine.Random.Range(0, NAMES.Length);
			}

			if (_idx >= NAMES.Length)
			{
				_idx = 0;
			}

			return NAMES[_idx++];
		}
	}
}