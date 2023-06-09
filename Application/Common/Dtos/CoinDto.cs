﻿using System.Text.Json.Serialization;
using Application.Common.Mappings;
using AutoMapper;
using Domain;

namespace Application.Common.Dtos;

public class CoinDto : IMapWith<Coin>
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;
    // Price in USD.
    [JsonPropertyName("priceUsd")]
    public double PriceUsd { get; set; }

    [JsonIgnore] 
    public int Quantity { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Coin, CoinDto>()
            .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(m => m.Symbol, opt => opt.MapFrom(s => s.Symbol))
            .ForMember(m => m.PriceUsd, opt => opt.MapFrom(s => s.PriceUsd));
    }
}