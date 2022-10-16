/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 Generated using http://xmltocsharp.azurewebsites.net/
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Xml2CSharp
{
	[XmlRoot(ElementName = "StartTime")]
	public class StartTime
	{
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "StopTime")]
	public class StopTime
	{
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "SaveInterval")]
	public class SaveInterval
	{
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "RandomSeed")]
	public class RandomSeed
	{
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "TimeSymbol")]
	public class TimeSymbol
	{
		[XmlAttribute(AttributeName = "symbol")]
		public string Symbol { get; set; }
	}

	[XmlRoot(ElementName = "Time")]
	public class Time
	{
		[XmlElement(ElementName = "StartTime")]
		public StartTime StartTime { get; set; }
		[XmlElement(ElementName = "StopTime")]
		public StopTime StopTime { get; set; }
		[XmlElement(ElementName = "SaveInterval")]
		public SaveInterval SaveInterval { get; set; }
		[XmlElement(ElementName = "RandomSeed")]
		public RandomSeed RandomSeed { get; set; }
		[XmlElement(ElementName = "TimeSymbol")]
		public TimeSymbol TimeSymbol { get; set; }
	}

	[XmlRoot(ElementName = "Description")]
	public class Description
	{
		[XmlElement(ElementName = "Title")]
		public string Title { get; set; }
		[XmlElement(ElementName = "Details")]
		public string Details { get; set; }
	}

	[XmlRoot(ElementName = "SpaceSymbol")]
	public class SpaceSymbol
	{
		[XmlAttribute(AttributeName = "symbol")]
		public string Symbol { get; set; }
	}

	[XmlRoot(ElementName = "Size")]
	public class Size
	{
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
		[XmlAttribute(AttributeName = "symbol")]
		public string Symbol { get; set; }
	}

	[XmlRoot(ElementName = "Condition")]
	public class Condition
	{
		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName = "boundary")]
		public string Boundary { get; set; }
	}

	[XmlRoot(ElementName = "BoundaryConditions")]
	public class BoundaryConditions
	{
		[XmlElement(ElementName = "Condition")]
		public List<Condition> Condition { get; set; }
	}

	[XmlRoot(ElementName = "Neighborhood")]
	public class Neighborhood
	{
		[XmlElement(ElementName = "Order")]
		public string Order { get; set; }
		[XmlElement(ElementName = "Distance")]
		public string Distance { get; set; }
	}

	[XmlRoot(ElementName = "Lattice")]
	public class Lattice
	{
		[XmlElement(ElementName = "Size")]
		public Size Size { get; set; }
		[XmlElement(ElementName = "BoundaryConditions")]
		public BoundaryConditions BoundaryConditions { get; set; }
		[XmlElement(ElementName = "Neighborhood")]
		public Neighborhood Neighborhood { get; set; }
		[XmlAttribute(AttributeName = "class")]
		public string Class { get; set; }
	}

	[XmlRoot(ElementName = "Space")]
	public class Space
	{
		[XmlElement(ElementName = "SpaceSymbol")]
		public SpaceSymbol SpaceSymbol { get; set; }
		[XmlElement(ElementName = "Lattice")]
		public Lattice Lattice { get; set; }
	}

	[XmlRoot(ElementName = "Constant")]
	public class Constant
	{
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
		[XmlAttribute(AttributeName = "symbol")]
		public string Symbol { get; set; }
	}

	[XmlRoot(ElementName = "Global")]
	public class Global
	{
		[XmlElement(ElementName = "Constant")]
		public List<Constant> Constant { get; set; }
	}

	[XmlRoot(ElementName = "CellType")]
	public class CellType
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "class")]
		public string Class { get; set; }
		[XmlElement(ElementName = "VolumeConstraint")]
		public VolumeConstraint VolumeConstraint { get; set; }
		[XmlElement(ElementName = "NeighborhoodReporter")]
		public List<NeighborhoodReporter> NeighborhoodReporter { get; set; }
		[XmlElement(ElementName = "Property")]
		public List<Property> Property { get; set; }
	}

	[XmlRoot(ElementName = "VolumeConstraint")]
	public class VolumeConstraint
	{
		[XmlAttribute(AttributeName = "strength")]
		public string Strength { get; set; }
		[XmlAttribute(AttributeName = "target")]
		public string Target { get; set; }
	}

	[XmlRoot(ElementName = "Input")]
	public class Input
	{
		[XmlAttribute(AttributeName = "scaling")]
		public string Scaling { get; set; }
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "Output")]
	public class Output
	{
		[XmlAttribute(AttributeName = "mapping")]
		public string Mapping { get; set; }
		[XmlAttribute(AttributeName = "symbol-ref")]
		public string Symbolref { get; set; }
	}

	[XmlRoot(ElementName = "NeighborhoodReporter")]
	public class NeighborhoodReporter
	{
		[XmlElement(ElementName = "Input")]
		public Input Input { get; set; }
		[XmlElement(ElementName = "Output")]
		public Output Output { get; set; }
	}

	[XmlRoot(ElementName = "Property")]
	public class Property
	{
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
		[XmlAttribute(AttributeName = "symbol")]
		public string Symbol { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "CellTypes")]
	public class CellTypes
	{
		[XmlElement(ElementName = "CellType")]
		public List<CellType> CellType { get; set; }
	}

	[XmlRoot(ElementName = "Contact")]
	public class Contact
	{
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
		[XmlAttribute(AttributeName = "type1")]
		public string Type1 { get; set; }
		[XmlAttribute(AttributeName = "type2")]
		public string Type2 { get; set; }
	}

	[XmlRoot(ElementName = "Interaction")]
	public class Interaction
	{
		[XmlElement(ElementName = "Contact")]
		public List<Contact> Contact { get; set; }
		[XmlAttribute(AttributeName = "default")]
		public string Default { get; set; }
	}

	[XmlRoot(ElementName = "MCSDuration")]
	public class MCSDuration
	{
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "MetropolisKinetics")]
	public class MetropolisKinetics
	{
		[XmlAttribute(AttributeName = "temperature")]
		public string Temperature { get; set; }
	}

	[XmlRoot(ElementName = "MonteCarloSampler")]
	public class MonteCarloSampler
	{
		[XmlElement(ElementName = "MCSDuration")]
		public MCSDuration MCSDuration { get; set; }
		[XmlElement(ElementName = "Neighborhood")]
		public Neighborhood Neighborhood { get; set; }
		[XmlElement(ElementName = "MetropolisKinetics")]
		public MetropolisKinetics MetropolisKinetics { get; set; }
		[XmlAttribute(AttributeName = "stepper")]
		public string Stepper { get; set; }
	}

	[XmlRoot(ElementName = "ShapeSurface")]
	public class ShapeSurface
	{
		[XmlElement(ElementName = "Neighborhood")]
		public Neighborhood Neighborhood { get; set; }
		[XmlAttribute(AttributeName = "scaling")]
		public string Scaling { get; set; }
	}

	[XmlRoot(ElementName = "CPM")]
	public class CPM
	{
		[XmlElement(ElementName = "Interaction")]
		public Interaction Interaction { get; set; }
		[XmlElement(ElementName = "MonteCarloSampler")]
		public MonteCarloSampler MonteCarloSampler { get; set; }
		[XmlElement(ElementName = "ShapeSurface")]
		public ShapeSurface ShapeSurface { get; set; }
	}

	[XmlRoot(ElementName = "ModelGraph")]
	public class ModelGraph
	{
		[XmlAttribute(AttributeName = "include-tags")]
		public string Includetags { get; set; }
		[XmlAttribute(AttributeName = "format")]
		public string Format { get; set; }
		[XmlAttribute(AttributeName = "reduced")]
		public string Reduced { get; set; }
	}

	[XmlRoot(ElementName = "Channel")]
	public class Channel
	{
		[XmlAttribute(AttributeName = "symbol-ref")]
		public string Symbolref { get; set; }
	}

	[XmlRoot(ElementName = "VtkPlotter")]
	public class VtkPlotter
	{
		[XmlElement(ElementName = "Channel")]
		public Channel Channel { get; set; }
		[XmlAttribute(AttributeName = "time-step")]
		public string Timestep { get; set; }
		[XmlAttribute(AttributeName = "mode")]
		public string Mode { get; set; }
	}

	[XmlRoot(ElementName = "Analysis")]
	public class Analysis
	{
		[XmlElement(ElementName = "ModelGraph")]
		public ModelGraph ModelGraph { get; set; }
		[XmlElement(ElementName = "VtkPlotter")]
		public VtkPlotter VtkPlotter { get; set; }
	}

	[XmlRoot(ElementName = "Cell")]
	public class Cell
	{
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "PropertyData")]
		public List<PropertyData> PropertyData { get; set; }
		[XmlElement(ElementName = "Center")]
		public string Center { get; set; }
		[XmlElement(ElementName = "Nodes")]
		public string Nodes { get; set; }
	}

	[XmlRoot(ElementName = "Population")]
	public class Population
	{
		[XmlElement(ElementName = "Cell")]
		public List<Cell> Cell { get; set; }
		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName = "size")]
		public string Size { get; set; }
	}

	[XmlRoot(ElementName = "PropertyData")]
	public class PropertyData
	{
		[XmlAttribute(AttributeName = "symbol-ref")]
		public string Symbolref { get; set; }
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "CellPopulations")]
	public class CellPopulations
	{
		[XmlElement(ElementName = "Population")]
		public List<Population> Population { get; set; }
	}

	[XmlRoot(ElementName = "MorpheusModel")]
	public class MorpheusModel
	{
		[XmlElement(ElementName = "Time")]
		public Time Time { get; set; }
		[XmlElement(ElementName = "Description")]
		public Description Description { get; set; }
		[XmlElement(ElementName = "Space")]
		public Space Space { get; set; }
		[XmlElement(ElementName = "Global")]
		public Global Global { get; set; }
		[XmlElement(ElementName = "CellTypes")]
		public CellTypes CellTypes { get; set; }
		[XmlElement(ElementName = "CPM")]
		public CPM CPM { get; set; }
		[XmlElement(ElementName = "Analysis")]
		public Analysis Analysis { get; set; }
		[XmlElement(ElementName = "CellPopulations")]
		public CellPopulations CellPopulations { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
	}

}
