using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleBall : MonoBehaviour
{
    private Table_shop_ball.Data m_ballTabData;

    private float m_prevBallPosY;

    private List<float> m_prevAngleList = new List<float>();
    private float m_rotAngle; // The rotate angle of a "car"

    private bool m_isInvincible = false;

    private Vector3 m_cameraOriginPos;

    private void Awake()
    {
        SetBallId(PlayerData.curBallId);
    }

    // Use this for initialization
    void Start()
    {
        GameObject camera = GameObject.Find("BattleScene/Camera3D");
        m_cameraOriginPos = camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //m_prevPrevPos = m_prevPos;
        //m_prevPos = gameObject.transform.localPosition;
        // something wrong? so strange: set localPosition but the next frame get localPosition is different

        // update pos and rotation
        BattleCanvas canvas = BattleScene.instance.GetCanvas();
        float ballPosY = canvas.GetBattleRoad().GetBallPosY();

        if (BattleScene.instance.IsBattleStarted())
        {
            if (BattleScene.instance.IsBallMoving())
            {
                // Calc the rotate angle
                Vector2 prevPos = new Vector2(Const.ballPosX - Time.deltaTime * canvas.GetMoveSpeed(), m_prevBallPosY);
                Vector2 pos = new Vector2(Const.ballPosX, ballPosY);
                Vector2 diff = pos - prevPos;
                float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                m_prevAngleList.Add(angle);

                List<float> lastAngles = new List<float>();
                int avgCount = 10;
                for (int i = 0; i < 10; ++i)
                {
                    int index = m_prevAngleList.Count - avgCount + i;
                    if (index >= 0)
                    {
                        lastAngles.Add(m_prevAngleList[index]);
                    }
                }
                m_prevAngleList = lastAngles;

                float totalAngle = 0.0f;
                foreach (float a in lastAngles)
                {
                    totalAngle += a;
                }
                float avgAngle = totalAngle / lastAngles.Count;
                m_rotAngle = avgAngle;
            }
        }
        else
        {
            m_rotAngle = 0.0f;
        }

        // Uphill slower, downhill faster
        //float maxScale = 1.5f;
        //float minScale = 0.5f;
        //if (m_rotAngle > 0) // Uphill
        //{
        //    BattleScene.instance.GetCanvas().SetSpeedScale(1.0f - (1.0f - minScale) * (m_rotAngle / 90.0f));
        //}
        //else if (m_rotAngle < 0) // Downhill
        //{
        //    BattleScene.instance.GetCanvas().SetSpeedScale(1.0f + (maxScale - 1.0f) * (m_rotAngle / -90.0f));
        //}
        //else
        //{
        //    BattleScene.instance.GetCanvas().SetSpeedScale(1.0f);
        //}

        // Is it need to limit the max drop speed? maybe unnecessary
        //float prevBallPosY = m_ballObj.transform.position.y;
        //if (prevBallPosY - ballPosY > Time.deltaTime * m_ballMaxDropSpeed)
        //{
        //    ballPosY = prevBallPosY - Time.deltaTime * m_ballMaxDropSpeed;
        //}
        gameObject.transform.localPosition = new Vector3(Const.ballPosX, ballPosY, 0);

        // Camera pos
        if (BattleScene.instance.IsBattleStarted())
        {
            GameObject camera = GameObject.Find("BattleScene/Camera3D");
            float prevCameraY = camera.transform.position.y;
            float curCameraY = m_cameraOriginPos.y + ballPosY * 0.8f;
            float maxCamaraY = prevCameraY + Time.deltaTime * 3.0f;
            float minCamaraY = prevCameraY - Time.deltaTime * 3.0f;
            curCameraY = Mathf.Clamp(curCameraY, minCamaraY, maxCamaraY);
            camera.transform.position = new Vector3(m_cameraOriginPos.x, curCameraY, m_cameraOriginPos.z);
        }

        // ball local rotation
        if (BattleScene.instance.IsBallMoving() && m_ballTabData.rotate)
        {
            float rotateRate = 360.0f;
            float rotateAngle = -rotateRate * Time.deltaTime;
            gameObject.transform.Rotate(0, 0, rotateAngle);
        }
        else
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, m_rotAngle);
        }

        m_prevBallPosY = ballPosY;
    }

    public void SetBallId(int ballId)
    {
        m_ballTabData = Table_shop_ball.data[ballId];
        Texture2D tex = Resources.Load<Texture2D>(string.Format("Balls/{0}", m_ballTabData.path));
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void SetIsInvincible(bool invincible, float time = 0.0f)
    {
        if (invincible)
        {
            if (time > 0.0f)
            {
                m_isInvincible = true;
                gameObject.GetComponent<Animator>().Play("Ball_invincible", 0, 0);

                IEnumerator _(float _time)
                {
                    yield return new WaitForSeconds(_time);
                    m_isInvincible = false;
                    //PlayInvincibleFinishAni();
                    gameObject.GetComponent<Animator>().Play("Ball_normal", 0, 0);
                }
                StartCoroutine(_(time));
            }
        }
        else
        {
            m_isInvincible = false;
        }
    }

    //private void PlayInvincibleFinishAni()
    //{
    //    gameObject.GetComponent<Animator>().Play("Ball_invincible_finish", 0, 0);

    //    IEnumerator _()
    //    {
    //        yield return new WaitForSeconds(1.0f);
    //        gameObject.GetComponent<Animator>().Play("Ball_normal", 0, 0);
    //    }
    //    StartCoroutine(_());
    //}

    //private void OnCollisionEnter2D(Collision2D param)
    //{
    //    BattleScene.instance.SetBattleEnded(true);
    //    param.rigidbody.isKinematic = true;
    //    param.otherRigidbody.isKinematic = true;
    //    //param.otherCollider.
    //    //if (other.gameObject.CompareTag("BattleBlock"))
    //    //{
    //    //    BattleScene.instance.SetBattleEnded(true);
    //    //    //this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    //    //    this.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
    //    //    //other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    //    //    other.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
    //    //}
    //}

    public void Reset()
    {
        // Sfx
        gameObject.transform.Find("CFX2_RockHit").gameObject.SetActive(false);
        gameObject.transform.Find("FX_Steam").gameObject.SetActive(false);

        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, 0);
        gameObject.GetComponent<Rigidbody>().ResetInertiaTensor();

        m_prevAngleList.Clear();

        GameObject camera = GameObject.Find("BattleScene/Camera3D");
        camera.transform.position = m_cameraOriginPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject.name == "FinishLine")
        {
            if (!BattleScene.instance.IsBattleEnded())
            {
                BattleScene.instance.SetBattleEnded(true);

                if (PlayerData.enableVibration)
                {
                    Handheld.Vibrate();
                }
            }
        }
        else if (otherGameObject.name == "Coin")
        {
            BattleScene.instance.GetCanvas().AddCoin();

            otherGameObject.transform.Find("CFX3 Stars Burst").gameObject.SetActive(true);

            otherGameObject.transform.Find("Icon").gameObject.SetActive(false);
            IEnumerator _(GameObject _obj)
            {
                yield return new WaitForSeconds(1.0f);
                BattleScene.instance.GetCanvas().RemoveItem(_obj);
            }
            StartCoroutine(_(otherGameObject));
        }
        else if (otherGameObject.name == "ChestKey")
        {
            PlayerData.GainChestKey();
            BattleScene.instance.GetCanvas().RemoveItem(otherGameObject);
        }
        else if (otherGameObject.name == "Checkpoint")
        {
            //BattleScene.instance.MakeBackupCanvas();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGameObject = collision.gameObject;
        if (otherGameObject.CompareTag("BattleBlock"))
        {
            if (!m_isInvincible)
            {
                if (!BattleScene.instance.IsBattleEnded())
                {
                    IEnumerator _()
                    {
                        yield return new WaitForSeconds(0.2f);
                        // Sfx
                        gameObject.transform.Find("FX_Steam").gameObject.SetActive(true);
                        gameObject.transform.Find("FX_Steam").GetComponent<ParticleSystem>().Play();
                    }
                    StartCoroutine(_());

                    gameObject.transform.Find("CFX2_RockHit").gameObject.SetActive(true);
                    gameObject.transform.Find("CFX2_RockHit").GetComponent<ParticleSystem>().Play();

                    BattleScene.instance.SetBattleEnded(false);

                    if (PlayerData.enableVibration)
                    {
                        Handheld.Vibrate();
                    }

                    BattleScene.instance.GetComponent<ShakeCamera>().enabled = true;
                }
            }

            // https://blog.csdn.net/RICKShaozhiheng/article/details/50779467
            //Vector2 origin = m_prevPrevPos;
            //Vector2 end = this.gameObject.transform.position;
            //Vector3 direction = end - origin;//射线方向
            //float distance = Vector3.Distance(origin, end);//射线检测距离
            //RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance);//发射射线，只检测与"Target"层的碰撞
            ////Debug.DrawRay(origin, direction, Color.red, 2);//绘制射线
            ////Debug.Assert(hit.collider != null, "未检测到起点");
            //for (int i = 0; i < 100; ++i)
            //    Debug.Log(string.Format("lsdfdsajfkldasjfkljdsklfjaskl {0} {1}", origin, end));
            //if (hit.collider != null)
            //{
            //    this.gameObject.transform.position = hit.point;//获得该碰撞点
            //}
        }
    }
}
