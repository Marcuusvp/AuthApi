using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApi.Aplicacao.Dtos;
using AuthApi.Dominio.Model;
using AutoMapper;

namespace AuthApi.Aplicacao.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<NovoUsuarioDto, User>();
            CreateMap<UsuarioDto, User>().ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
        }
        
    }
}