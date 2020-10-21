﻿using System;
using System.Collections.Generic;

namespace calculations
{

	using ConnectionGene = genome.ConnectionGene;
	using Genome = genome.Genome;
	using NodeGene = genome.NodeGene;


	public class Calculator
	{

		private List<Node> input_nodes = new List<Node>();
		private List<Node> hidden_nodes = new List<Node>();
		private List<Node> output_nodes = new List<Node>();

		public Calculator(Genome g)
		{
			data_structures.Listset<NodeGene> nodes = g.Nodes;
			data_structures.Listset<ConnectionGene> cons = g.Connections;

			Dictionary<int, Node> nodeHashMap = new Dictionary<int, Node>();

			foreach (NodeGene n in nodes.Data)
			{

				Node node = new Node(n.X);
				nodeHashMap[n.Innovation_number] = node;

				if (n.X <= 0.1)
				{
					input_nodes.Add(node);
				}
				else if (n.X >= 0.9)
				{
					output_nodes.Add(node);
				}
				else
				{
					hidden_nodes.Add(node);
				}
			}

			hidden_nodes.Sort((x,y)=>x.CompareTo(y));

			foreach (ConnectionGene c in cons.Data)
			{
				NodeGene from = c.From;
				NodeGene to = c.To;

				Node node_from = nodeHashMap[from.Innovation_number];
				Node node_to = nodeHashMap[to.Innovation_number];

				Connection con = new Connection(node_from, node_to);
				con.Weight = c.Weight;
				con.Enabled = c.Enabled;

				node_to.Connections.Add(con);
			}
		}

		

		public virtual double[] calculate(params double[] input)
		{

			if (input.Length != input_nodes.Count)
			{
				throw new Exception("Data doesnt fit");
			}
			for (int i = 0; i < input_nodes.Count; i++)
			{
				input_nodes[i].Output = input[i];
			}
			foreach (Node n in hidden_nodes)
			{
				n.calculate();
			}

			double[] output = new double[output_nodes.Count];
			for (int i = 0; i < output_nodes.Count; i++)
			{
				output_nodes[i].calculate();
				output[i] = output_nodes[i].Output;
			}
			return output;
		}

	}

}