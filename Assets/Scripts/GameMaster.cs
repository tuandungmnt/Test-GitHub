using UnityEngine;
using UnityEngine.UI;
using System;

public class GameMaster : MonoBehaviour {
    public GameObject blockPrefab;
    public Canvas actionMenu;

    public Vector2 rootPosition;
    public GameObject[] block;
    Vector3 mousePos;
    bool pressed = false;
    int[] st = new int[40];
    int n = 0;
    bool[] clicked = new bool[40]; 
    int[] num = new int[40];
    System.Random rand = new System.Random();

    void Start() {
        block = new GameObject[35];        

        for (int i = 0; i < 5; ++i)
        for (int j = 0; j < 7; ++j) {
            int x = i * 7 + j;
            Vector2 tmp = new Vector2(i * 100, j * 100);
            block[x] = Instantiate(blockPrefab) as GameObject;
            Vector3 pos = new Vector3();
            pos.x = 20 + i * 100;
            pos.y = -300 + j * 100;
            block[x].transform.position = pos;
            block[x].transform.SetParent(actionMenu.transform,false);

            num[x] = rand.Next(1, 5);
            num[x] = (int) Math.Pow(2, num[x]);
            block[x].GetComponentInChildren<Text>().text = num[x].ToString() ;
        }

        for (int i = 0; i < 35; ++i) clicked[i] = false;
        Vector3 p = block[0].transform.position;
        Debug.Log(p.x + " " + p.y);
    }

    bool MouseInBlock(float x, float y) {
        if (mousePos.x < x - 40 || mousePos.x > x + 40) return false;
        if (mousePos.y < y - 40 || mousePos.y > y + 40) return false;
        return true;
    }

    bool checkAdject(int x,int y) {
        Vector2 xx = new Vector2(x / 7, x % 7);
        Vector2 yy = new Vector2(y / 7, y % 7);
        if (Math.Abs(xx.x - yy.x) <= 1 && Math.Abs(xx.y - yy.y) <= 1) return true;
        return false;
    }

    void click(int x) {
        if (clicked[x] == true) return;
        clicked[x] = true;
        block[x].GetComponent<Image>().color = new Color(0, 0, 0);
    }

    void unclick(int x) {
        if (clicked[x] == false) return;
        clicked[x] = false;
        block[x].GetComponent<Image>().color = new Color(255, 255, 255);
    }

    void Update() {
        mousePos = Input.mousePosition;

        //Debug.Log("Mouse " + mousePos.x + " " + mousePos.y);

        if (Input.GetMouseButtonDown(0)) pressed = true;
        if (Input.GetMouseButtonUp(0)) pressed = false;

        if (pressed) {
            int b = -1;
            for (int i = 0; i < 35; ++i) {
                Vector3 p = block[i].transform.position;
                if (MouseInBlock(p.x, p.y)) {
                    b = i;
                    break;
                }
            }
            Debug.Log("Chi vao o: " + b + " " + n);
            
            if (b == -1) return;
            if (n > 1 && st[n-2] == b) {        
                unclick(st[n-1]);
                n--;
                Debug.Log("Khong Hieu Lam " + n);
                return;
            }

            if (clicked[b] == false && (n == 0 || (checkAdject(st[n-1], b) && num[b] >= num[st[n-1]]))) {
                click(b);
                st[n] = b;
                n++;
                Debug.Log("Tang " + n);
            }

        }
        else {
            if (n>0) {
                int sum = 0;
                for (int i = 0; i < n; ++i) {
                    sum+=num[st[i]];
                    block[st[i]].GetComponentInChildren<Text>().text = "";
                    num[st[i]]=0;
                    unclick(st[i]);
                }
                int s = 2;
                while (s <= sum) s *= 2;
                s /= 2;
                block[st[n-1]].GetComponentInChildren<Text>().text = s.ToString();
                num[st[n-1]] = s;

                Debug.Log("Vua xoa "+ n);
            }
            n = 0;
            Debug.Log("Process Delete");
        }

    }
}

