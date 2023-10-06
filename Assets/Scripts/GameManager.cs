using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<TileController> ListTileController => m_listTileController;
    [SerializeField] private List<TileController> m_listTileController;
    public int Turn { get; set; }

    public GameObject gameOverPanel;
    public GameObject winnerPanel;
    public TextMeshProUGUI winnerText;


    private void Awake()
    {
        if (instance != this)
            instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this.gameObject);
    }

    public (bool, TileState) HasWinner()
    {
        foreach (var tile in m_listTileController)
        {
            if (tile.state == TileState.None) continue;

            foreach (var direction in Enum.GetValues(typeof(Direction)))
            {
                var next = tile.GetNextTile((Direction)direction);
                if (!next) continue;

                if (next.state != tile.state) continue;

                var lasTile = next.GetNextTile((Direction)direction);
                if (!lasTile) continue;

                if (lasTile.state != tile.state) continue;

                return (true, tile.state);

            }//Direction Foreach
        }//TileController list foreach

        return (false, TileState.None);
    }//HasWinner

    public bool GameOver()
    {
        for (int i = 0; i < m_listTileController.Count; i++)
        {
            if (m_listTileController[i].state == TileState.None || HasWinner().Item1)
                return false;
        }

        return true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

}//Class
