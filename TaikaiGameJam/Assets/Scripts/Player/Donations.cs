﻿using UnityEngine;

public class Donations 
{
    [Header("Earn amount")]
    public float m_AmtPerTree = 0.5f;
    public float m_AmtPerPlant = 0.05f;

    //a certain percentage u might gain or lose
    public Vector2 m_MinMaxDonationPercentage = new Vector2(0.8f, 1.2f);
    //popularity / by a certain number * 100
    public int m_NumberOfDonorPerPercentage = 200; 

    public int GetMonthlyDonation(int currentTreeNumber, int currentPlantNumber, int popularityCount)
    {
        //trees/plant * amt of money * a certain popularity percentage * a random percentage
        float amt = m_AmtPerTree * currentTreeNumber + currentPlantNumber * m_AmtPerPlant;
        amt = amt * (1.0f + ((int)popularityCount / m_NumberOfDonorPerPercentage));
        amt = amt * Random.Range(m_MinMaxDonationPercentage.x, m_MinMaxDonationPercentage.y);

        return Mathf.RoundToInt(amt);
    }
}