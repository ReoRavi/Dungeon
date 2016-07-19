using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    // 배경 객체
    public BackGroundScroll backGround;
    // 배경 스크롤 속도
    public float backGroundScrollSpeed;
    // 몬스터 리스폰 객체
    EnemyRespawn respawn;
    // 현재시간, 이전 시간
    private float respawnTime;
    // 몬스터 레벨이 갱신되는 리스폰 횟수
    private int levelUpCount;
    // 리스폰 시간 간격
    public float respawnInterval;
    // 몬스터 리스폰 레벨
    public int respawnLevel;
    // 몬스터가 리스폰된 횟수
    public int respawnCount;
    // 몬스터 이동 속도
    public float moveSpeed;

    // 플레이중인가
    public bool bIsPlay;

    // 게임 UI
    public GameObject playUI;
    // 게임 끝 UI
    public GameObject endUI;

    // 점수 UI
    ScoreRenewal scoreUI;

	// Use this for initialization
	void Start () {
        backGroundScrollSpeed = 1F;
        respawn = GetComponent<EnemyRespawn>();
        respawnTime = 0;
        levelUpCount = 3;
        respawnInterval = 4;
        respawnLevel = 0;
        respawnCount = 1;
        moveSpeed = 0.05F;

        bIsPlay = true;

        scoreUI = GetComponent<ScoreRenewal>();
    }
	
	// Update is called once per frame
	void Update () {

        if (bIsPlay)
        {
            // 배경 스크롤
            backGround.Scroll(backGroundScrollSpeed);
            // 게임 루프
            GameLoop();
        }
    }

    public void GameEnd(GameObject player)
    {
        bIsPlay = false;
        player.SetActive(false);
        playUI.SetActive(false);
        endUI.SetActive(true);

        ScoreRenewal endScore = endUI.GetComponent<ScoreRenewal>();
        endScore.Renewal(respawnCount - 1);
    }

    void GameLoop()
    {
        // 시간 계산
        respawnTime += Time.deltaTime;

        // 시간 간격 마다 몬스터 리스폰
        if (respawnTime >= respawnInterval)
        {
            // 배경 속도 증가
            if (backGroundScrollSpeed <= 15F)
            {
                backGroundScrollSpeed += 0.2F;
            }

            // 몬스터가 소환될 때 마다 난이도를 조절한다.

            // 몬스터 이동 속도 증가
            if ((respawnCount % levelUpCount) == 0 && moveSpeed <= 0.22F)
            {
                moveSpeed += 0.01F;
            }

            // 몬스터 리스폰 간격 조절
            if ((respawnCount % (levelUpCount * 3)) == 0 && respawnInterval > 0.5F)
            {
                respawnInterval -= 0.5F;
            }

            // 몬스터 리스폰 레벨 조정
            if ((respawnCount % (levelUpCount * 10)) == 0 && respawnLevel <= 0)
            {
                respawnLevel++;
            }

            respawnTime = 0;
            respawnCount++;

            scoreUI.Renewal(respawnCount - 1);

            // 리스폰 레벨 0
            if (respawnLevel == 0)
            {
                respawn.Respawn(Random.Range(0, 4), Random.Range(-1, 2), moveSpeed);
            }
            // 리스폰 레벨 1
            else
            {
                int firstRespawnCount = Random.Range(-1, 2);
                int secondRespawnCount = Random.Range(-1, 2);

                if (firstRespawnCount == secondRespawnCount)
                {
                    while (firstRespawnCount == secondRespawnCount)
                    {
                        secondRespawnCount = Random.Range(-1, 2);
                    }
                }

                Debug.Log(firstRespawnCount);
                Debug.Log(secondRespawnCount);

                respawn.Respawn(Random.Range(0, 4), firstRespawnCount, moveSpeed);
                respawn.Respawn(Random.Range(0, 4), secondRespawnCount, moveSpeed);
            }
        }
    }
}
