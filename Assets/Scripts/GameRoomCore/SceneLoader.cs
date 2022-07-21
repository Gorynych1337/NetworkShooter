using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviourPunCallbacks
{
    public static SceneLoader Instanse;
    
    public ControllerManager NowController => nowController;
    private ControllerManager nowController;

    void Awake()
    {
        if (Instanse)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instanse = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex != 1)
            return;

        if (nowController)
        {
            PhotonNetwork.Destroy(nowController.gameObject);
        }
        
        ControllerManager controllerManagerClone = 
            PhotonNetwork.Instantiate("ControllerManager", Vector3.zero, Quaternion.identity).GetComponent<ControllerManager>();

        nowController = controllerManagerClone.GetComponent<ControllerManager>();
    }
    
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Going back to main menu");
    }
}
