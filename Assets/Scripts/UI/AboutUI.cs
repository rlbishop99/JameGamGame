using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutUI : MonoBehaviour
{
    public void ToItch() {

        Application.OpenURL("https://plasmalot.itch.io/");

    }

    public void ToGitHub() {

        Application.OpenURL("https://github.com/rlbishop99/");

    }

    public void ToPersonal() {

        Application.OpenURL("https://rockobishop.com/");

    }
}
