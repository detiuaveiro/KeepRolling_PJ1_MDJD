using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class PageController : MonoBehaviour
{

    public List<PageData> page_content;
    public VideoPlayer videoPlayer;
    public Text text;
    public GameObject back_button;
    public GameObject next_button;
    private int page_index;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.clip = page_content[0].video_clip;
        text.text = page_content[0].page_text;
        if (page_content.Count > 1) {
            next_button.SetActive(true);
        }
        page_index = 0;
    }

    public void SetButtonsActive() 
    {
        if (page_index == page_content.Count - 1)
        {
            next_button.SetActive(false);
            back_button.SetActive(true);
        }
        else if (page_index == 0)
        {
            next_button.SetActive(true);
            back_button.SetActive(false);
        }
        else {
            next_button.SetActive(true);
            back_button.SetActive(true);
        }
    }

    public void NextPage() 
    {
        if (page_index + 1 < page_content.Count) {
            page_index++;
            SetButtonsActive();
            videoPlayer.clip = page_content[page_index].video_clip;
            text.text = page_content[page_index].page_text;
        }
    }

    public void BackPage()
    {
        if (page_index - 1 >= 0)
        {
            page_index--;
            SetButtonsActive();
            videoPlayer.clip = page_content[page_index].video_clip;
            text.text = page_content[page_index].page_text;
        }
    }
}
