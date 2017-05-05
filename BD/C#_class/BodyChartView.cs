﻿using System;

public class BodyChartView
{
    private int ID;
    private String ImageBody;
    private int OrderImage;

    public int ID
    {
        get { return ID; }
        set { ID = value; }

    }
    public String ImageBody
    {
        get { return ImageBody; }
        set { ImageBody = value; }

    }
    public int OrderImage
    {
        get { return OrderImage; }
        set { OrderImage = value; }

    }



    public BodyChartView()
	{
	}

    private void submitBodyChartView(BodyChartView B)
    {
        if (!verifySGBDConnection())
            return;
        SqlCommand cmd = new SqlCommand();
        cmd.ComandText = "INSERT INTO BodyChartView (ID, ImageBody, OrderImage) " + "VALUES (@ID, @ImageBody, @OrderImage)";
        cmd.Parameters.AddWithValue("@ID", B.ID);
        cmd.Parameters.AddWithValue("@ImageBody", B.ImageBody);
        cmd.Parameters.AddWithValue("@OrderImage", B,OrderImage);
        cmd.Connection = cn;

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Insert BodyChartView in database. \n ERROR MESSAGE: \n" + ex.Message);
        }
        finally
        {
            cn.Close();
        }

    }

    private void selectBodyChartView()
    {
        if (!verifySGBDConnection())
            return;

        SqlCommand cmd = new SqlCommand("SELECT * FROM BodyChartView", cn);
        SqlDataReader reader = cmd.ExecuteReader();
        listBox1.Items.Clear();
        while (reader.Read())
        {
            BodyChartView B = new BodyChartView();
            B.ID = reader["ID"].ToString();
            B.ImageBody = reader["ImageBody"].ToString();
            B.OrderImage = reader["OrderImage"].ToString();
            listBox1.Items.Add(B);

        }
        cn.Close();



    }
}
