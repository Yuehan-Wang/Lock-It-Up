using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject Object;
    public GameObject pot1;
    public GameObject pot2;

    public GameObject start;
    public GameObject lose;
    public GameObject replay;
    public GameObject success;
    public GameObject logo;
    private ArrayList pot2Arr;

    public int columnNum = 20;
    public int rowNum = 10;

    private ArrayList potArr;
    private bool hasStarted = false;
    private bool gameOver = false;
    void Start()
    {
        start.GetComponent<StartGame>().controller = this;
        replay.GetComponent<StartGame>().controller = this;
        potArr = new ArrayList();
        pot2Arr = new ArrayList();
        for (int rowIndex = 0; rowIndex < rowNum; rowIndex ++){
            ArrayList tmp = new ArrayList();
            for(int columnIndex = 0; columnIndex < columnNum; columnIndex++){
                Item item = CreatePot(pot1, rowIndex, columnIndex);
                tmp.Add(item);
            }
            potArr.Add(tmp);
        }
        
    }

    public void StartGame(){
        music.PlaySound("startSound");
        hasStarted = true;
        start.SetActive(false);
        logo.SetActive(false);
        gameOver = false;
        success.SetActive(false);
        lose.SetActive(false);
        replay.SetActive(false);

        Object.SetActive(true);
        MoveObject(Random.Range(3, rowNum - 3), Random.Range(3, columnNum - 3));

        for (int rowIndex = 0; rowIndex < rowNum; rowIndex ++){
            for(int columnIndex = 0; columnIndex < columnNum; columnIndex++){
                Item item = GetPot(rowIndex, columnIndex);
                item.movable = true;
            }
        }
        for (int i = 0; i < pot2Arr.Count; i++){
            Item pot2 = pot2Arr[i] as Item;
            Destroy(pot2.gameObject);
        }
        pot2Arr = new ArrayList();
    }

    Item GetPot(int rowIndex, int columnIndex){
        if(rowIndex < 0 || rowIndex > rowNum - 1 || columnIndex < 0 || columnIndex > columnNum - 1){
            return null;
        }
        ArrayList tmp = potArr[rowIndex] as ArrayList;
        Item item = tmp[columnIndex] as Item;
        return item;
    }

    void MoveObject(int rowIndex, int columnIndex){
        Item item = Object.GetComponent<Item>();
        item.Goto(rowIndex, columnIndex);
    }

    public void Select(Item item){
        if(!hasStarted || gameOver){
            return;
        }
        if(item.movable){
            Item pot2Item = CreatePot(pot2, item.rowIndex, item.columnIndex);
            pot2Arr.Add(pot2Item);
            item.movable = false;
            ArrayList steps = FindSteps();
            if(steps.Count > 0){
                int index = Random.Range(0, steps.Count);
                Vector2 v = (Vector2)steps[index];
                MoveObject((int)v.y, (int)v.x);
                if (Escaped()){
                    gameOver = true;
                    music.PlaySound("loseSound");
                    lose.SetActive(true);
                    replay.SetActive(true);
                    Object.SetActive(false);
                }
            }else{
                music.PlaySound("winSound");
                gameOver = true;
                success.SetActive(true);
                replay.SetActive(true);
            }
        }
        
    }

    bool Escaped(){
        Item item = Object.GetComponent<Item>();
        int rowIndex = item.rowIndex;
        int columnIndex = item.columnIndex;
        if(rowIndex == 0 || rowIndex == rowNum - 1 || columnIndex == 0 || columnIndex == columnNum - 1){
            return true;
        }
        return false;
    }

    bool Movable(Vector2 v){
        Item item = GetPot((int)v.y, (int)v.x);
        if(item == null){
            return false;
        }
        return item.movable;
    }
    ArrayList FindSteps(){
        ArrayList steps = new ArrayList();
        Vector2 v = new Vector2();
        Item item = Object.GetComponent<Item>();
        int rowIndex = item.rowIndex;
        int columnIndex = item.columnIndex;
        //left
        v.y = rowIndex;
        v.x = columnIndex - 1;
        if(Movable(v)){
            steps.Add(v);
        }
        //right
        v.y = rowIndex;
        v.x = columnIndex + 1;
        if(Movable(v)){
            steps.Add(v);
        }
        //top
        v.y = rowIndex + 1;
        v.x = columnIndex;
        if(Movable(v)){
            steps.Add(v);
        }
        //buttom
        v.y = rowIndex - 1;
        v.x = columnIndex;
        if(Movable(v)){
            steps.Add(v);
        }
        //topleft topright
        v.y = rowIndex + 1;
        if(rowIndex % 2 == 1){
            v.x = columnIndex - 1;
        }else{
            v.x = columnIndex + 1;
        }
        if(Movable(v)){
            steps.Add(v);
        }
        //buttomleft buttomright
        v.y = rowIndex - 1;
        if(rowIndex % 2 == 1){
            v.x = columnIndex - 1;
        }else{
            v.x = columnIndex + 1;
        }
        if(Movable(v)){
            steps.Add(v);
        }
        return steps;
    }

    Item CreatePot(GameObject pot, int rowIndex, int columnIndex){
        GameObject o = Instantiate(pot) as GameObject;
        o.transform.parent = this.transform;
        Item item = o.GetComponent<Item>();
        item.Goto(rowIndex, columnIndex);
        item.game = this;
        return item;
    }
}
