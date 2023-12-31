using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestDAD : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox, textFinished, textUnFinished;
    [SerializeField] private int questGoal = 10;
    [SerializeField] private int levelToLoad;
    [SerializeField] private AudioClip questCompletedSound;

    private AudioSource sourceAudio;
    private Animator anim;
    private bool LevelIsLoading = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sourceAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

                if (other.GetComponent<PlayerMovement>().cherriesFound >= questGoal)
            {
                sourceAudio.PlayOneShot(questCompletedSound);
                dialogueBox.SetActive(true);
                textFinished.SetActive(true);
                Invoke("LoadNextLevel", 5.0f);
                LevelIsLoading = true;
            }

            else
            {
                dialogueBox.SetActive(true);
                textUnFinished.SetActive(true);
            }
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !LevelIsLoading)
        {
            dialogueBox.SetActive(false);
            textFinished.SetActive(false);
            textUnFinished.SetActive(false);

        }
    }

}
