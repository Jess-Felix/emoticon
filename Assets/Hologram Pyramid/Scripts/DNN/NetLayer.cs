using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNetwork;
using EmoticonStorage;

public class NetLayer : MonoBehaviour {

	//Neural Network Variables
	private const double MinimumError = 0.1;
//	private const TrainingType TrType = TrainingType.MinimumError;
	private static NeuralNet net;
	public static List<DataSet> dataSets; 
	public static bool trained;
	public String storageFilename = null;
		
	private int collectedDatasets = 0;
	private const int maxNumberOfDatasets = 50; // 10 neutral, 10 happy, 10 sad, 10 angry, 10 relaxed

	public ExpressiveFeaturesExtraction mExpressiveFeatures; 

	// Use this for initialization
	void Start () {
		if(storageFilename == null || storageFilename.Equals(""))
			storageFilename = "expressions_data";

		//Initialize the network 
		net = new NeuralNet(4, 4, 4); // hiddenSize until 6 to improve precision
		
		dataSets = (List<DataSet>) StorageHandler.LoadData(storageFilename); 
		if(dataSets == null){
			dataSets = new List<DataSet>();
		} else {
			collectedDatasets = maxNumberOfDatasets;
		}
	}

	public void Train(double joy, double calm, double anger, double sadness){ 
		double[] C = {(double)mExpressiveFeatures.mFeatureEnergy, 
						(double) mExpressiveFeatures.mFeatureSymmetrySpatial,
						(double) mExpressiveFeatures.mFeatureSymmetrySpread,
						(double) mExpressiveFeatures.mFeatureSpatialExtent,
					//	(double) mExpressiveFeatures.mFeatureHeadLeaning
						};

		double[] v = {joy, calm, anger, sadness};

		dataSets.Add(new DataSet(C, v));

		collectedDatasets++;
		
		if (!trained && collectedDatasets >= maxNumberOfDatasets) {
			print ("Start training of the network."); 

			StorageHandler.SaveData(dataSets, storageFilename);

			TrainNetwork();
		} else{ // Update emotion to be trained
			switch(collectedDatasets){
				case (10):
				case (20):
                case (30):
                case (40):
                    mExpressiveFeatures.NextEmotion();
				break;
			}
		}
	}

	public double[] compute(double[] vals)
	{
		double[] result = net.Compute(vals);
		return result;
	}

	public static void TrainNetwork()
	{
		net.Train(dataSets, MinimumError);
		trained = true;
		print ("Trained!"); 
	}

	public void DeleteDatabase(){
		StorageHandler.DeleteFile(storageFilename);
	}
}
