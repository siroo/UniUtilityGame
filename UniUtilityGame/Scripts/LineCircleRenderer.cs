﻿using UnityEngine;
using UnityEngine.Scripting;

namespace Game
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineCircleRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer m_lineRenderer = null; // 円を描画するための LineRenderer
        [SerializeField] private float m_radius = 0;    // 円の半径
        [SerializeField] private float m_lineWidth = 0;    // 円の線の太さ

        [SerializeField] private float m_duration = 0; // スケール演出の再生時間（秒）
        [SerializeField] private float m_from = 0; // スケール演出の開始値
        [SerializeField] private float m_to = 0; // スケール演出の終了値
        [SerializeField] private bool isUpdate = true;

        [SerializeField] private int m_Segments;

        private float m_elapedTime;

        private void Reset()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
        }

        private void Awake()
        {
            InitLineRenderer();
        }

        private void Update()
        {
            if (isUpdate)
            {
                m_elapedTime += Time.deltaTime;

                var amount = m_elapedTime % m_duration / m_duration;
                var scale = Mathf.Lerp(m_from, m_to, amount);

                transform.localScale = new Vector3(scale, scale, 1);
            }
        }

        private void InitLineRenderer()
        {
            var segments = m_Segments;

            m_lineRenderer.startWidth = m_lineWidth;
            m_lineRenderer.endWidth = m_lineWidth;
            m_lineRenderer.positionCount = segments;
            m_lineRenderer.loop = true;
            m_lineRenderer.useWorldSpace = false; // transform.localScale を適用するため

            var points = new Vector3[segments];

            for (int i = 0; i < segments; i++)
            {
                var rad = Mathf.Deg2Rad * (i * 360f / segments);
                var x = Mathf.Sin(rad) * m_radius;
                var y = Mathf.Cos(rad) * m_radius;
                points[i] = new Vector3(x, y, 0);
            }

            m_lineRenderer.SetPositions(points);
        }
    }
}
