using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HowToPlayUI : MonoBehaviour
{
    public GameObject[] pages;
    private int pageIndex;

    public TextMeshProUGUI pageNum;

    private void Awake() {
        pageIndex = 0;
    }

    private void Update() {
        
        pageNum.text = (pageIndex + 1) + " of " + pages.Length;

    }

    public void Back() {

        if(pageIndex == 0) {

            return;

        } else {

            pages[pageIndex].SetActive(false);
            pageIndex--;
            pages[pageIndex].SetActive(true);

        }

    }

    public void Forward() {

        if(pageIndex == pages.Length - 1) {

            return;

        } else {

            pages[pageIndex].SetActive(false);
            pageIndex++;
            pages[pageIndex].SetActive(true);

        }

    }

    public void Close() {

        pages[pageIndex].SetActive(false);
        pageIndex = 0;
        pages[pageIndex].SetActive(true);
        
        gameObject.SetActive(false);

    }
}
